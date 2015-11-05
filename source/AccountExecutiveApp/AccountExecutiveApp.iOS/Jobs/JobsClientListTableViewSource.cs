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
		private readonly UITableViewController _parentController;

		private List<List<Job>> JobsByClient;

		public JobsClientListTableViewSource ( UITableViewController parentVC, IEnumerable<Job> jobs )
		{
		    JobsByClient = getJobsByClient(jobs);
		
			_parentController = parentVC;
		}
         
        /*
         Traverse the jobs, if we find a client name we have not seen before we add it to the first dimension of "JobsByClient". Otherwise 
         just add the job to the second dimension of the existing client index. This assumes client names are unique. 
         */
        private List<List<Job>> getJobsByClient(IEnumerable<Job> jobs )
        {
            List<List<Job>> jobsByClient = new List<List<Job>>();
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
                    jobsByClient[clientIndex].Add(jobToAdd);
                else
                    jobsByClient.Add(new List<Job>() { jobToAdd });
            }

            return jobsByClient;
	    }
         
        public override nint NumberOfSections(UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (JobsByClient != null)
				return JobsByClient.Count();
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

		    if (JobsByClient != null)
			{
		        cell.TextLabel.Text = JobsByClient[(int)indexPath.Item][0].ClientName;
					
		        if (cell.DetailTextLabel != null)
			    {
			        int numJobs = JobsByClient[(int)indexPath.Item].Count();
			        int jobsProposed = 0, jobCallouts = 0;

			        foreach (Job job in JobsByClient[(int)indexPath.Item])
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

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			JobsListViewController vc = (JobsListViewController)_parentController.Storyboard.InstantiateViewController ("JobsListViewController");
			vc.setJobs( JobsByClient[(int)indexPath.Item] );
			_parentController.ShowViewController ( vc, _parentController );
		}
	}
}

