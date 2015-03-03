﻿using System;

namespace SiSystems.ClientApp.SharedModels
{
    public class ContractProposal
    {
        /// <summary>
        /// Gets or sets the Id of the <see cref="Consultant"/> who will receive the proposal
        /// </summary>
        public int ConsultantId { get; set; }
        
        /// <summary>
        /// Gets or sets the id of the client who is initiating the proposal
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the proposed start date.
        /// </summary>
        /// <remarks>
        /// Can be back-dated by up to 30 days by an AE or
        /// even further by match guid
        /// </remarks>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Gets or sets the proposed end date.
        /// </summary>
        /// <remarks>
        /// Should occur after <see cref="StartDate"/>
        /// </remarks>
        public DateTime EndDate { get; set; }

        public Decimal RateToConsultant { get; set; }

        public Decimal Fee { get; set; }
        
        public string TimesheetApproverEmailAddress { get; set; }
        public string ContractApproverEmailAddress { get; set; }
    }
}
