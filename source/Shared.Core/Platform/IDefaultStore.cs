using System;

namespace Shared.Core.Platform
{
	public interface IDefaultStore
	{
		string Username { get; set; }

        string TokenExpiresAt { get; set; }

        int TokenExpiresIn { get; set; }

        string TokenIssuedAt { get; set; }
	}
}

