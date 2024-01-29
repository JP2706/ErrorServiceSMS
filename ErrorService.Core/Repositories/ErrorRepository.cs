using ErrorService.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorService.Core.Repositories
{
    public class ErrorRepository
    {
        public List<Error> GetLastErrors(int Interval)
        {
            return new List<Error> 
            { 
                new Error{Id = 1, Message = "Błąd test1", Date = DateTime.Now },
                new Error{Id = 2, Message = "Błąd test2", Date = DateTime.Now },
            };
        }
    }
}
