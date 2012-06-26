using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BadBehavior.Rules
{
    public class BlackList : IRule
    {
        // Blacklisted user agents
        // These user agent strings occur at the beginning of the line.
        private static readonly string[] spambots0 = new string[] {
            "<sc",                      // XSS exploit attempts
            "8484 Boston Project",      // video poker/porn spam
            "adwords",                  // referrer spam
            "autoemailspider",          // spam harvester
            "blogsearchbot-martin",     // from honeypot
            "CherryPicker",             // spam harvester
            "core-project/",            // FrontPage extension exploits
            "Diamond",                  // delivers spyware/adware
            "Digger",                   // spam harvester
            "ecollector",               // spam harvester
            "EmailCollector",           // spam harvester
            "Email Siphon",             // spam harvester
            "EmailSiphon",              // spam harvester
            "Forum Poster",             // forum spambot
            "grub crawler",             // misc comment/email spam
            "HttpProxy",                // misc comment/email spam
            "Internet Explorer",        // XMLRPC exploits seen
            "ISC Systems iRc",          // spam harvester
            "Jakarta Commons",          // customised spambots
            "Java 1.",                  // unidentified robots
            "Java/1.",                  // unidentified robots
            "libwww-perl",              // unidentified robots
            "LWP",                      // unidentified robots
            "Microsoft URL",            // unidentified robots
            "Missigua",                 // spam harvester
            "MJ12bot/v1.0.8",           // malicious botnet
            "Movable Type",             // customised spambots
            "Mozilla ",                 // malicious software
            "Mozilla/2",                // malicious software
            "Mozilla/4.0(",             // from honeypot
            "Mozilla/4.0+(compatible;+",// suspicious harvester
            "MSIE",                     // malicious software
            "NutchCVS",                 // unidentified robots
            "Nutscrape/",               // misc comment spam
            "OmniExplorer",             // spam harvester
            "Opera/9.64(",              // comment spam bot
            "psycheclone",              // spam harvester
            "PussyCat ",                // misc comment spam
            "PycURL",                   // misc comment spam
            "Python-urllib",            // commonly abused
            // WP 2.5 now has Flash; FIXME
            // "Shockwave Flash",    // spam harvester
            "Super Happy Fun ",         // spam harvester
            "TrackBack/",               // trackback spam
            "user",                     // suspicious harvester
            "User Agent: ",             // spam harvester
            "User-Agent: ",             // spam harvester
            "WebSite-X Suite",          // misc comment spam
            "Winnie Poh",               // Automated Coppermine hacks
            "Wordpress",                // malicious software
            "\"",                       // malicious software
        };

        // These user agent strings occur anywhere within the line.
        private static readonly string[] spambots = new string[] {
            "\r",                   // A really dumb bot
            "; Widows ",            // misc comment/email spam
            "a href=",              // referrer spam
            "Bad Behavior Test",    // Add this to your user-agent to test BB
            "compatible ; MSIE",    // misc comment/email spam
            "compatible-",          // misc comment/email spam
            "DTS Agent",            // misc comment/email spam
            "Email Extractor",      // spam harvester
            "Gecko/25",             // revisit this in 500 years
            "grub-client",          // search engine ignores robots.txt
            "hanzoweb",             // very badly behaved crawler
            "Indy Library",         // misc comment/email spam
            "MSIE 7.0;  Windows NT 5.2",    // Cyveillance
            "Murzillo compatible",  // comment spam bot
            ".NET CLR 1)",          // free poker, etc.
            "POE-Component-Client", // free poker, etc.
            "Turing Machine",       // www.anonymizer.com abuse
            "Ubuntu/9.25",          // comment spam bot
            "unspecified.mail",     // stealth harvesters
            "User-agent: ",         // spam harvester/splogger
            "WebaltBot",            // spam harvester
            "WISEbot",              // spam harvester
            "WISEnutbot",           // spam harvester
            "Windows NT 4.0;)",     // wikispam bot
            "Windows NT 5.0;)",     // wikispam bot
            "Windows NT 5.1;)",     // wikispam bot
            "Windows XP 5",         // spam harvester
            "WordPress/4.01",       // pingback spam
            "Xedant Human Emulator",// spammer script engine
            "\\\\)",                // spam harvester
        };

        // These are regular expression matches.
        private static readonly Regex[] spambotsRegex = new Regex[] {
            new Regex("^[A-Z]{10}$"),    // misc email spam
            // msnbot is using this fake user agent string now
            // new Regex("^Mozilla...[05]$", RegexOptions.IgnoreCase),    // fake user agent/email spam
            new Regex("[bcdfghjklmnpqrstvwxz ]{8,}"),
            // new Regex("/(;\){1,2}$/"),        // misc spammers/harvesters
            // new Regex("/MSIE.*Windows XP/"),    // misc comment spam
        };

        private static readonly Error EBlacklist = new Error(
            "17f4e8c8", 403, Explanations.Blacklisted,
            "User-Agent was found on blacklist"
        );


        public RuleResult Validate(Package package)
        {
            if (package.Request.UserAgent == null) return RuleResult.Continue;

            foreach (string spambot in spambots0)
                if (package.Request.UserAgent.StartsWith(spambot))
                    package.Throw(this, EBlacklist);

            foreach (string spambot in spambots)
                if (package.Request.UserAgent.Contains(spambot))
                    package.Throw(this, EBlacklist);

            foreach (Regex spambot in spambotsRegex)
                if (spambot.IsMatch(package.Request.UserAgent))
                    package.Throw(this, EBlacklist);

            return RuleResult.Continue;
        }
    }
}
