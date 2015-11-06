using System;
using UIKit;
using Foundation;
using SiSystems.SharedModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;

namespace AccountExecutiveApp.iOS
{
	public class JobsClientListTableViewSource : UITableViewSource
	{
		private readonly UITableViewController _parentController;

		private IEnumerable<IGrouping<string, Job>> JobsByClient;

		public JobsClientListTableViewSource ( UITableViewController parentVC, IEnumerable<IGrouping<string, Job>> jobsGroupedByClient )
		{
		    //JobsByClient = getJobsByClient(jobs);
		    JobsByClient = jobsGroupedByClient;
		    //jobs.GroupBy(job => job.ClientName);
		
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
			var cell = tableView.DequeueReusableCell ("RightDetailCell") ??
                new RightDetailCell(UITableViewCellStyle.Value1, "RightDetailCell");

		    if (JobsByClient == null)
		        return cell;

            var clientGrouping = JobsByClient.ElementAt((int)indexPath.Item);
		    UpdateCell(clientGrouping, cell);
		    
		    return cell;
		}

	    private void UpdateCell(IGrouping<string, Job> clientGrouping, UITableViewCell cell)
	    {
	        cell.TextLabel.Text = clientGrouping.Key;

	        if (cell.DetailTextLabel == null) return;
	        
            var numJobs = clientGrouping.Count();
	        var jobsProposed = clientGrouping.Count(j => j.isProposed);
	        var jobCallouts = clientGrouping.Count(j => j.hasCallout);

	        cell.DetailTextLabel.Text = string.Format("{0}/{1}/{2}", numJobs, jobsProposed, jobCallouts);
	    }

	    public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var vc = (JobsListViewController)_parentController.Storyboard.InstantiateViewController ("JobsListViewController");
			vc.setJobs( JobsByClient.ElementAt((int)indexPath.Item) );
			_parentController.ShowViewController ( vc, _parentController );
		}
	}
}

