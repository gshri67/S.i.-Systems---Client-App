using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using SiSystems.ClientApp.SharedModels;
using UIKit;

namespace ClientApp.iOS
{
	partial class ContractorViewController : UIViewController
	{
		public ContractorViewController (IntPtr handle) : base (handle)
		{
		}

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            //get our list of specializations to display
            var consultantGroups = GetConsultantGroups();

            //set the source for our table's data
            SpecializationTable.Source = new ContractsTableViewSource(consultantGroups);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

	    private IEnumerable<ConsultantGroup> GetConsultantGroups()
	    {
	        return new List<ConsultantGroup>
	        {
	            new ConsultantGroup()
	            {
	                Specialization = "Project Management",
                    Consultants = new List<ConsultantSummary>
                    {
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Fred",
                            LastName = "Flintstone"
                        }),
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Barney",
                            LastName = "Rubble"
                        }),
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Wilma",
                            LastName = "Flintstone"
                        })
                    }
	            },
                new ConsultantGroup()
                {
	                Specialization = "SW Development",
                    Consultants = new List<ConsultantSummary>
                    {
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Fred",
                            LastName = "Flintstone"
                        }),
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Barney",
                            LastName = "Rubble"
                        }),
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Wilma",
                            LastName = "Flintstone"
                        })
                    }
	            },
                new ConsultantGroup()
                {
	                Specialization = "Business Analyst",
                    Consultants = new List<ConsultantSummary>
                    {
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Fred",
                            LastName = "Flintstone"
                        }),
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Barney",
                            LastName = "Rubble"
                        }),
                        new ConsultantSummary(new Consultant
                        {
                            FirstName = "Wilma",
                            LastName = "Flintstone"
                        })
                    }
	            }
	        };
	    }

	    public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
	}
}
