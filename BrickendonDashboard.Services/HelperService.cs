using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrickendonDashboard.Domain.Contracts;
using BrickendonDashboard.Shared.Domain.Constants;
using BrickendonDashboard.Shared.Domain.Helpers;
using BrickendonDashboard.Domain.Exceptions;

namespace BrickendonDashboard.Services
{
	public class HelperService : IHelperService
	{
		public void ValidateUserName(string userName)
		{
			if (!UtilityHelper.IsEmailFormatValid(userName))
			{
				throw new CustomException(ErrorConstant.ErrorInvalidUserId);
			}
			if (!UtilityHelper.IsEmailLengthValid(userName))
			{
				throw new CustomException(ErrorConstant.ErrorUserIdTooLong);
			}
		}
	}
}
