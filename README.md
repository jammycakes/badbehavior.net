Bad Behavior .NET
=================
This is a .NET port of the original [Bad Behavior WordPress plugin][1]. It is
built as an HTTP module, so you can just drop it into any existing website
and enable it with a simple change to your web.config file.

Bad Behavior prevents spammers from ever delivering their junk, and in many
cases, from ever reading your site in the first place.

 [1]: http://bad-behavior.ioerror.us/

Description
-----------
Welcome to a whole new way of keeping your blog, forum, guestbook, wiki or
content management system free of link spam. Bad Behavior .NET is a port
of the original PHP-based solution for blocking link spam and the robots
which deliver it.

Thousands of sites large and small, like SourceForge, GNOME, the U.S.
Department of Education, and many more, trust Bad Behavior to help reduce
incoming link spam and malicious activity.

Bad Behavior complements other link spam solutions by acting as a gatekeeper,
preventing spammers from ever delivering their junk, and in many cases, from
ever reading your site in the first place. This keeps your site's load down,
makes your site logs cleaner, and can help prevent denial of service
conditions caused by spammers.

Bad Behavior also transcends other link spam solutions by working in a
completely different, unique way. Instead of merely looking at the content of
potential spam, Bad Behavior analyzes the delivery method as well as the
software the spammer is using. In this way, Bad Behavior can stop spam attacks
even when nobody has ever seen the particular spam before.

Bad Behavior is designed to work alongside existing spam prevention services
to increase their effectiveness and efficiency. Whenever possible, you should
run it in combination with a more traditional spam prevention service.

Bad Behavior works on virtually any ASP.NET-based Web software package, whether
you use Web Forms or ASP.NET MVC. It consists of a simple HTTP module that you
can just drop into your web application and configure with a couple of lines in
your web.config file.

Installing and configuring Bad Behavior is simple and takes only a few minutes.
In most cases, no configuration at all is needed. Simply turn it on and stop
worrying about spam!

The core of Bad Behavior is free software released under the GNU Lesser General
Public License, version 3, or at your option, any later version.

Status
------
Bad Behavior .NET is currently in "alpha" status. All the tests from the
original PHP version have now been ported to .NET, although more testing is
required and no guarantees are given as to this project's stability, so use
it at your own risk.

The API is not yet officially stable, but only minor changes are anticipated
between now and the 1.0 release.

Building
--------
You will need the following software installed on your computer:

 * The .NET framework SDK version 4.0 or later
 * Python 3.2 or later to run the build scripts
 * A local SQL Server 2008 Express named instance called SQLEXPRESS
   with a database called BadBehavior is needed to run the unit tests.
   The user account running the script should be granted dbo
   privileges to this database to create the tables and stored
   procedures.

Contributing
------------
Bad Behavior .NET uses Bad Behavior 2.7 as a reference. The source code for
this is available from the Bad Behavior SVN repository at:

 * http://plugins.svn.wordpress.org/bad-behavior/

Pull requests will be accepted either via Git (on Github) or Mercurial
(on Bitbucket). The respective URLs are:

 * https://github.com/jammycakes/badbehavior.net
 * https://bitbucket.org/jammycakes/badbehavior.net

Before contributing a pull request, please note the following code conventions:

 1. We use four spaces for indentation, not tabs. Pull requests that mix tabs
    and spaces in the same file will be rejected with extreme prejudice.
 2. Keep line lengths below 100 characters where possible.
 3. Bad Behavior is intended to be deployed to the end user as a single DLL
    with no dependencies, so don't add extra projects to the solution and
    don't add third-party components, whether through NuGet or otherwise.
 4. An exception means that your method can not do what its name says that it
    does. Absence of an exception means that your method has done what its
    name says that it does. Do not deviate from these conventions.
 5. Terminology should normally match that in the original PHP Bad Behavior
    reference implementation. Exceptions should be noted in porting.txt.
 6. Git users: STRICTLY no octopus merges.
 7. Mercurial users: STRICTLY no named branches. Commit only to default.
 8. Changes to .gitignore must be reflected in .hgignore and vice versa.
 9. Every check-in should:

    1. make only one change
    2. have a commit summary that accurately describes that change
    3. compile without errors or warnings.
