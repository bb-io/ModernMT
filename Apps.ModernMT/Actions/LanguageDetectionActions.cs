﻿using Apps.ModernMT.Models.LanguageDetection.Requests;
using Apps.ModernMT.Models.LanguageDetection.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using ModernMT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.ModernMT.Actions
{
    [ActionList]
    public class LanguageDetectionActions
    {
        [Action("Detect language", Description = "Detect language of the text")]
        public DetectLanguageResponse DetectLanguage(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DetectLanguageRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            var language = mmt.DetectLanguage(input.Text);
            return new DetectLanguageResponse()
            {
                Language = language.Language
            };
        }

        [Action("Detect multiple languages", Description = "Detect multiple languages")]
        public DetectMultipleLanguagesResponse DetectMultipleLanguages(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DetectMultipleLanguagesRequest input)
        {
            var mmt = new ModernMTClient(authenticationCredentialsProviders);
            var languages = mmt.DetectLanguage(input.Texts);
            return new DetectMultipleLanguagesResponse()
            {
                Languages = languages.Select(l => l.Language).ToList()
            };
        }
    }
}
