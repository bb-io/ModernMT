using Apps.ModernMT.Dtos;
using Apps.ModernMT.Models.Memories.Responses;
using Apps.ModernMT.Models.Requests;
using Apps.ModernMT.Models.Responses;
using Apps.ModernMT.Models.Translations.Requests;
using Apps.ModernMT.Models.Translations.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using ModernMT;
using ModernMT.Model;
using Newtonsoft.Json.Linq;
using System;

namespace Apps.ModernMT
{
    [ActionList]
    public class Actions
    {
        #region Language detection

        [Action("Detect language", Description = "Detect language of the text")]
        public DetectLanguageResponse DetectLanguage(AuthenticationCredentialsProvider authenticationCredentialsProvider, 
            [ActionParameter] DetectLanguageRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var language = mmt.DetectLanguage(input.Text);
            return new DetectLanguageResponse()
            {
                Language = language.Language
            };
        }

        [Action("Detect multiple languages", Description = "Detect multiple languages")]
        public DetectMultipleLanguagesResponse DetectMultipleLanguages(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] DetectMultipleLanguagesRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var languages = mmt.DetectLanguage(input.Texts);
            return new DetectMultipleLanguagesResponse()
            {
                Languages = languages.Select(l =>  l.Language).ToList()
            };
        }

        #endregion

        #region Translations

        [Action("Translate text", Description = "Translate into specified language")]
        public TranslationResponse TranslateIntoLanguage(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslationRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text);
            return new TranslationResponse()
            {
                TranslatedText = translation.TranslationText
            };
        }

        [Action("Translate multiple texts", Description = "Translate multiple texts into specified language")]
        public MultipleTranslationResponse TranslateMultiple(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] MultipleTranslationRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var translations = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Texts);
            return new MultipleTranslationResponse()
            {
                TranslatedTexts = translations.Select(t => t.TranslationText).ToList()
            };
        }

        [Action("Translate text with hints", Description = "Translate text with hints")]
        public TranslationResponse TranslateWithHints(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslationWithHintsRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, input.Hints.ToArray());
            return new TranslationResponse()
            {
                TranslatedText = translation.TranslationText
            };
        }

        [Action("Translate text with context", Description = "Translate text with specified context")]
        public TranslationResponse TranslateWithContext(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslationWithContextRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, null, input.Context);
            return new TranslationResponse()
            {
                TranslatedText = translation.TranslationText
            };
        }

        [Action("Get translation options", Description = "Get translation options")]
        public TranslationOptionsResponse GetTranslationOptions(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslationOptionsRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var translation = mmt.Translate(input.SourceLanguage, input.TargetLanguage, input.Text, null, null, new TranslateOptions()
            {
                AltTranslations = input.NumberOfOptions
            });
            return new TranslationOptionsResponse()
            {
                TranslatedText = translation.TranslationText,
                AlternativeOptions = translation.AltTranslations.ToList()
            };
        }

        #endregion

        #region Context vectors

        [Action("Get context vector from text", Description = "Get context vector from text")]
        public ContextVectorResponse GetContextVectorFromText(AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] ContextVectorRequest input)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var contextVectors = mmt.GetContextVector(input.SourceLanguage, input.TargetLanguages, input.Text);
            return new ContextVectorResponse()
            {
               ContextVectors = contextVectors
            };
        }

        #endregion

        #region Memories

        [Action("Get all memories", Description = "Get all memories")]
        public AllMemoriesResponse GetAllMemories(AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var mmt = new ModernMTService(authenticationCredentialsProvider.Value);
            var memories = mmt.Memories.List();
            return new AllMemoriesResponse()
            {
                Memories = memories.Select(m => new MemoryDto()
                {
                    Name = m.Name,
                    CreatedOn = m.CreationDate,
                    Id = m.Id
                })
            };
        }

        #endregion
    }
}
