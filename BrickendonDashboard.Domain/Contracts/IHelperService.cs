using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickendonDashboard.Domain.Contracts
{
	public interface IHelperService
	{
		void ValidateUserName(string userName);
	}
}
