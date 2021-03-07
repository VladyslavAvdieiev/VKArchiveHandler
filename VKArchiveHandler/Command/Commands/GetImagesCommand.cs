using Command.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Command.Commands {
    public class GetImagesCommand : ICommand {
        public event Action<CommandEventArgs> OnLog;
        public event Action<CommandEventArgs> OnErrorLog;

        private static readonly string imagePattern = "//sun";
        private static readonly string urlGroupName = "url";
        private static readonly string urlPattern = $"class='attachment__link' href='(?<{urlGroupName}>.+)'";

        public void Execute(Arguments args) {
            var htmlPaths = LocateAllHtmlFiles(args.SourceLocation);
            var htmlContents = LoadHtmlFileContent(htmlPaths);
            var attachmentLinks = GetAttachmentLinks(htmlContents);
            var imageUrls = attachmentLinks.Where(x => x.Contains(imagePattern));
            DownloadPhotos(args.OutputFolder ?? GetUserId(args.SourceLocation), imageUrls);
        }

        public void DownloadPhotos(string outputFolder, IEnumerable<string> urls) {
            var client = new WebClient();
            if (!Directory.Exists(outputFolder)) {
                Directory.CreateDirectory(outputFolder);
            }
            OnLog?.Invoke(new CommandEventArgs($"{urls.Distinct().Count()} unique images remaining", null));
            foreach (var url in urls) {
                var imageName = GetImageFullName(url);
                try {
                    client.DownloadFile(new Uri(url), Path.Combine(outputFolder, imageName));
                } catch (Exception) {
                    OnErrorLog?.Invoke(new CommandEventArgs($"ERR: {imageName}", url));
                    continue;
                }
                OnLog?.Invoke(new CommandEventArgs($"OK:  {imageName}", url));
            }
        }

        private IEnumerable<string> LocateAllHtmlFiles(string sourceLocation) {
            return Directory.GetFiles(sourceLocation, "*.html", SearchOption.AllDirectories);
        }

        private IEnumerable<string> LoadHtmlFileContent(IEnumerable<string> htmlPaths) {
            foreach (var htmlPath in htmlPaths) {
                yield return File.ReadAllText(htmlPath);
            }
        }

        private IEnumerable<string> GetAttachmentLinks(IEnumerable<string> htmlContents) {
            foreach (var htmlContent in htmlContents) {
                foreach (Match match in Regex.Matches(htmlContent, urlPattern)) {
                    yield return match.Groups[urlGroupName].Value;
                }
            }
        }

        private string GetUserId(string sourceLocation) {   // C:\Users\..\Archive\messages\123456789
            var splittedPath = sourceLocation.Split('\\');
            return splittedPath[^1];                        // 123456789
        }

        private string GetImageFullName(string url) {                                               // sun9-XX.userapi.com/impg/.../name.type?size=XXXXxXXXX
            var rightSideGroupName = "type";
            var rightSidePattern = $"/(?<{rightSideGroupName}>.+)\\?size";
            var rightSide = Regex.Match(url, rightSidePattern).Groups[rightSideGroupName].Value;    // sun9-XX.userapi.com/impg/.../name.type
            var splittedSide = rightSide.Split('/');
            return splittedSide[^1];                                                                // name.type
        }
    }
}
