using System.Text.RegularExpressions;
using RequirementAI.Business.Interfaces.Refinement;
using RequirementAI.Contract.Dto.LLMDtos;
using RequirementAI.Persistence.Entities;

namespace RequirementAI.Business.Services.Refinement;

public class UserStoryLanguageValidator : IUserStoryLanguageValidator
{
    private static readonly Regex GermanCharacters = new("[äöüß]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex GermanWords = new(
        @"\b(als|ich|möchte|damit|und|oder|der|die|das|den|dem|ein|eine|einen|einem|einer|nicht|kann|soll|für|mit|von|zu|im|in|auf|aus)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex EnglishWords = new(
        @"\b(as|i|want|so|that|and|or|the|a|an|not|can|should|for|with|from|to|in|on|out)\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex GermanStoryStructure = new(
        @"^\s*Als\b.+\bmöchte\s+ich\b.+\bdamit\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex EnglishStoryStructure = new(
        @"^\s*As\b.+\bI\s+want\b.+\bso\s+that\b",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex EnglishStoryFragments = new(
        @"(^\s*As\b|\bI\s+want\b|\bso\s+that\b)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex GermanStoryFragments = new(
        @"(^\s*Als\b|\bmöchte\s+ich\b|\bdamit\b)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public string? GetCorrectionInstruction(
        UserStory input,
        IEnumerable<UserStoryForLLMDto> outputStories)
    {
        var language = DetectLanguage($"{input.Title} {input.Description}");
        var descriptions = outputStories.Select(story => story.Description).ToList();

        return language switch
        {
            UserStoryLanguage.German when descriptions.Any(description =>
                !GermanStoryStructure.IsMatch(description) || EnglishStoryFragments.IsMatch(description)) =>
                "The INPUT language is German. Rewrite every natural-language field in German. " +
                "Every user story description must use the structure 'Als [Persona] möchte ich [Aktion/Fähigkeit], " +
                "damit [Nutzen/Mehrwert].' Do not use English structural phrases or mix German and English.",

            UserStoryLanguage.English when descriptions.Any(description =>
                !EnglishStoryStructure.IsMatch(description) || GermanStoryFragments.IsMatch(description)) =>
                "The INPUT language is English. Rewrite every natural-language field in English and do not mix languages. " +
                "Every description must express the persona, desired capability, and resulting benefit.",

            _ => null
        };
    }

    private static UserStoryLanguage DetectLanguage(string input)
    {
        if (GermanCharacters.IsMatch(input))
            return UserStoryLanguage.German;

        var germanScore = GermanWords.Matches(input).Count;
        var englishScore = EnglishWords.Matches(input).Count;

        if (germanScore >= 3 && germanScore >= englishScore)
            return UserStoryLanguage.German;

        if (englishScore >= 3 && englishScore > germanScore)
            return UserStoryLanguage.English;

        return UserStoryLanguage.Unknown;
    }

    private enum UserStoryLanguage
    {
        Unknown,
        German,
        English
    }
}
