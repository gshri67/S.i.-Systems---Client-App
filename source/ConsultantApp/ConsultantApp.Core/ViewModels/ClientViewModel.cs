using System;
using System.Collections.Generic;

using SiSystems.ClientApp.SharedModels;

namespace ConsultantApp.Core.ViewModels
{
	public class ClientViewModel
	{
		public ClientViewModel ()
		{
		}

		public List<Client> loadClientsForConsultant( Consultant consultant )
		{
			List<Client> clients = new List<Client>();

			for (int i = 0; i < 5; i++) 
			{
				Client c = new Client ();
				c.name = "Nexen";

				c.projectCodes = new List<ProjectCode> ();

				ProjectCode p1 = new ProjectCode ();
				p1.code = "P-234";

				if (i == 4) 
				{
					c.name = "Cenovus";
					p1.code = "P-456";

					ProjectCode p2 = new ProjectCode ();
					p2.code = "P-777";

					c.projectCodes.Add(p2);
				}


				c.projectCodes.Add(p1);

				clients.Add (c);
			}

			return clients;
		}
	}
}

