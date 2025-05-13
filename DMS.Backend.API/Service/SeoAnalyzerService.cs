using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DMS.Backend.Services
{
    public class SeoAnalysisResult
    {
        public string Score { get; set; } // "Good", "OK", "Poor"
        public string Color { get; set; } // "green", "orange", "red"
        public string[] Recommendations { get; set; } = Array.Empty<string>();
    }

    public class SeoAnalyzerService
    {
        public SeoAnalysisResult AnalyzeDocument(string title, string content, string tags, string focusKeyword)
        {
            var result = new SeoAnalysisResult();
            var recommendations = new System.Collections.Generic.List<string>();

            // Clean content (remove HTML tags for analysis)
            var plainContent = Regex.Replace(content, "<[^>]+>", "");
            var wordCount = plainContent.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;

            // 1. Title Length (Yoast recommends 40-60 characters)
            if (string.IsNullOrEmpty(title))
            {
                recommendations.Add("Add a title to improve SEO.");
            }
            else if (title.Length < 40)
            {
                recommendations.Add("Title is too short. Aim for 40-60 characters.");
            }
            else if (title.Length > 60)
            {
                recommendations.Add("Title is too long. Keep it under 60 characters.");
            }

            // 2. Content Length (Yoast recommends 300+ words)
            if (wordCount < 300)
            {
                recommendations.Add($"Content is too short ({wordCount} words). Aim for at least 300 words.");
            }

            // 3. Focus Keyword Usage
            if (string.IsNullOrEmpty(focusKeyword))
            {
                recommendations.Add("Specify a focus keyword for better SEO.");
            }
            else
            {
                var keywordCount = CountKeywordOccurrences(plainContent, focusKeyword);
                var keywordDensity = wordCount > 0 ? (keywordCount / (double)wordCount) * 100 : 0;
                if (keywordCount == 0)
                {
                    recommendations.Add("Focus keyword not found in content.");
                }
                else if (keywordDensity < 0.5)
                {
                    recommendations.Add($"Keyword density is low ({keywordDensity:F1}%). Aim for 0.5-2.5%.");
                }
                else if (keywordDensity > 2.5)
                {
                    recommendations.Add($"Keyword density is high ({keywordDensity:F1}%). Avoid over-optimization.");
                }

                if (!title.ToLower().Contains(focusKeyword.ToLower()))
                {
                    recommendations.Add("Include the focus keyword in the title.");
                }
            }

            // 4. Tags
            if (string.IsNullOrEmpty(tags))
            {
                recommendations.Add("Add tags to improve discoverability.");
            }

            // Calculate Score
            int issueCount = recommendations.Count;
            if (issueCount == 0)
            {
                result.Score = "Good";
                result.Color = "green";
            }
            else if (issueCount <= 2)
            {
                result.Score = "OK";
                result.Color = "orange";
            }
            else
            {
                result.Score = "Poor";
                result.Color = "red";
            }

            result.Recommendations = recommendations.ToArray();
            return result;
        }

        private int CountKeywordOccurrences(string content, string keyword)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(keyword))
                return 0;
            var regex = new Regex($@"\b{Regex.Escape(keyword)}\b", RegexOptions.IgnoreCase);
            return regex.Matches(content).Count;
        }
    }
}