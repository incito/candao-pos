using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace PackingConsole
{
     public interface IReleaseNoteLoader
    {
        string GetNote();
    }

    class JiraReleaseNoteLoader : IReleaseNoteLoader
    {
        private readonly string _jiraUrl;
        private readonly Regex _listRegex;
        private readonly Regex _descRegex;
        private readonly Regex _noteRegex;

        public JiraReleaseNoteLoader(string jiraUrl)
        {
            _jiraUrl = jiraUrl;
            _listRegex = new Regex(@"<a id=""release-notes-(\d+)"" class=""subText"" href=""(.*)"" title=""View release notes"">发行报告</a>");
            _descRegex = new Regex("<span class=\"versionBanner-description\">(.*)</span>");
            _noteRegex = new Regex(@"<textarea rows=""40"" cols=""120"" id=""editcopy"">([\s\S]*?)</textarea>");
        }

        public string GetNote()
        {
            using (var wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
               var listHtml =  wc.DownloadString(_jiraUrl);
               var matches = _listRegex.Matches(listHtml);
                if (matches.Count == 0)
                    throw new ApplicationException("no road map found.");
                var noteUrl = matches[0].Result("$2");
                var uri = new Uri(_jiraUrl);
                noteUrl = "http://"+ uri.Host + noteUrl;

                var desc = string.Empty;
                matches = _descRegex.Matches(listHtml);
                if (matches.Count > 0)
                {
                    desc = "<h1>" + matches[0].Result("$1") + "</h1>";
                }

                var noteHtml = wc.DownloadString(noteUrl);
                var match = _noteRegex.Match(noteHtml);
                if (match.Success)
                {
                    var note = match.Result("$1");
                    note = Regex.Replace(note, @"版本说明(.+)版本 (\d|\.)+", "");
                    return desc + note;
                }
                return desc;
            }
        }
    }

   
}
