using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BadBehaviour.Validators
{
	/* ====== Browser checks ====== */

	public class Browser : IValidation
	{
		public ValidationResult Validate(Package request)
		{
			// See core.inc.php in BB original.

			string ua = request.Request.UserAgent.ToLowerInvariant();

			if (ua.Contains("; msie")) {
				request.IsBrowser = true;
				if (ua.Contains("opera"))
					ValidateOpera(request);
				else
					ValidateMSIE(request);
			}
			else if (ua.Contains("konqueror")) {
				request.IsBrowser = true;
				ValidateKonqueror(request);
			}
			else if (ua.Contains("opera")) {
				request.IsBrowser = true;
				ValidateOpera(request);
			}
			else if (ua.Contains("safari")) {
				request.IsBrowser = true;
				ValidateSafari(request);
			}
			else if (ua.Contains("lynx")) {
				request.IsBrowser = true;
				ValidateLynx(request);
			}
			else if (ua.Contains("movabletype")) {
				request.IsBrowser = false;
				ValidateMovableType(request);
			}
			else if (ua.StartsWith("mozilla")) {
				request.IsBrowser = true;
				ValidateMozilla(request);
			}

			return ValidationResult.Continue;
		}

		/* ====== Browser checks ====== */

		// See browser.inc.php in original BB.

		private void ValidateKonqueror(Package request)
		{
		}

		private void ValidateLynx(Package request)
		{
		}

		private void ValidateMozilla(Package request)
		{
		}

		private void ValidateMSIE(Package request)
		{
		}

		private void ValidateOpera(Package request)
		{
		}

		private void ValidateSafari(Package request)
		{
		}


		/* ====== Movable Type ====== */

		// See movabletype.inc.php in BB original.

		private void ValidateMovableType(Package request)
		{
		}
	}
}
