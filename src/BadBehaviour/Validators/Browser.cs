﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehaviour.Validators
{
	/* ====== Browser checks ====== */

	public class Browser : IValidation
	{
		public ValidationResult Validate(Package package)
		{
			// See core.inc.php in BB original.

			if (package.UserAgentI.Contains("; msie")) {
				package.IsBrowser = true;
				if (package.UserAgentI.Contains("opera"))
					ValidateOpera(package);
				else
					ValidateMSIE(package);
			}
			else if (package.UserAgentI.Contains("konqueror")) {
				package.IsBrowser = true;
				ValidateKonqueror(package);
			}
			else if (package.UserAgentI.Contains("opera")) {
				package.IsBrowser = true;
				ValidateOpera(package);
			}
			else if (package.UserAgentI.Contains("safari")) {
				package.IsBrowser = true;
				ValidateSafari(package);
			}
			else if (package.UserAgentI.Contains("lynx")) {
				package.IsBrowser = true;
				ValidateLynx(package);
			}
			else if (package.UserAgentI.Contains("movabletype")) {
				package.IsBrowser = false;
				ValidateMovableType(package);
			}
			else if (package.UserAgentI.StartsWith("mozilla")) {
				package.IsBrowser = true;
				ValidateMozilla(package);
			}

			return ValidationResult.Continue;
		}

		/* ====== Browser checks ====== */

		// See browser.inc.php in original BB.

		private void ValidateKonqueror(Package package)
		{
		}

		private void ValidateLynx(Package package)
		{
		}

		private void ValidateMozilla(Package package)
		{
		}

		private void ValidateMSIE(Package package)
		{
		}

		private void ValidateOpera(Package package)
		{
		}

		private void ValidateSafari(Package package)
		{
		}


		/* ====== Movable Type ====== */

		// See movabletype.inc.php in BB original.

		private void ValidateMovableType(Package package)
		{
		}
	}
}
