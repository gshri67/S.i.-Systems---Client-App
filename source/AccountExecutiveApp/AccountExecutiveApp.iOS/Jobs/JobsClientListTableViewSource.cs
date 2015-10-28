using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace AccountExecutiveApp.iOS
{
	public class JobsClientListTableViewSource : UITableViewSource
	{
        private List<List<Job>> jobsByClient;

		public JobsClientListTableViewSource ( IEnumerable<Job> jobs )
		{
            jobsByClient = new List<List<Job>>();
            List<Job> jobsList = jobs.ToList();

            for (int i = 0; i < jobs.Count(); i++)
            {
                bool clientAlreadyAdded = false;
                int clientIndex = -1;

                for (int j = 0; j < jobsByClient.Count; j++)
                {
                    if (jobsByClient[j][0].ClientName == jobsList[i].ClientName)
                    {
                        clientAlreadyAdded = true;
                        clientIndex = j;
                    }
                }

                Job jobToAdd = jobsList[i];

                if (clientAlreadyAdded)
                    jobsByClient[clientIndex].Add( jobToAdd );
                else
                {
                    jobsByClient.Add(new List<Job>() { jobToAdd });
                }
            }
		}

		public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
            if (jobsByClient != null)
                return jobsByClient.Count();
            else
    			return 0;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("RightDetailCell");

			if (cell == null)
			{
				Console.WriteLine ("creating new cell");

				cell = new RightDetailCell (UITableViewCellStyle.Value1, "RightDetailCell");
			}

            if (jobsByClient != null)
            {
                cell.TextLabel.Text = jobsByClient[(int)indexPath.Item][0].ClientName;

                if (cell.DetailTextLabel != null)
                {
                    int numJobs = jobsByClient[(int)indexPath.Item].Count();
                    int jobsProposed = 0, jobCallouts = 0;

                    foreach (Job job in jobsByClient[(int)indexPath.Item])
                    {
                        if (job.hasCallout)
                            jobCallouts++;
                        if (job.isProposed)
                            jobsProposed++;
                    }

                    cell.DetailTextLabel.Text = numJobs + "/" + jobsProposed + "/" + jobCallouts;
                }
            }

			return cell;
		}

	}
}

