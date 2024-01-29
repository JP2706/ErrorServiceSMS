using ErrorService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErrorService.Core
{
    public class GenrateHtmlSms
    {
        public string GenrateError(List<Error> errors, int interval)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            if (!errors.Any())
                return string.Empty;

            var message = $@"Błędy z ostatnich {interval} minut.";

            foreach (var error in errors) 
            {
                message += $"Wiadomość: {error.Message},Data: {error.Date}.";
            }

            message += "Wiadmosć wygenerowana automatycznie";

            return message;
        }
    }
}
