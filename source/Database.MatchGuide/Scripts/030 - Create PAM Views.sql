/*
	************************************************************ Create PAM View ******************************************************
*/

create view [dbo].[VWHistoricalReplicatedPam10501]
as

select 
		a.AgreementID,
		a.agreementsubid as ContractID,
		pam.DATE1,
		PAYTYPID
from	
		Historical_SISGP_PAM10501 pam
		inner join Agreement a on (a.agreementsubid = (LEFT(replace(pam.[jobnumbr], substring(pam.[jobnumbr], 1, charindex('_', pam.[jobnumbr])), ''), charindex('_', replace(pam.[jobnumbr], substring(pam.[jobnumbr], 1, charindex('_', pam.[jobnumbr])), '')) - 1))
		and a.agreementtype = (select pl.PicklistId from PickList pl inner join Picktype pt  on pt.picktypeid = pl.picktypeid  and pt.type = 'agreementtype'  and pl.Title = 'contract' where  pl.inactive = 0 and pt.inactive = 0 and pl.verticalid  = -1))
UNION ALL
select 
		a.AgreementID,
		a.agreementsubid as ContractID,
		pam.DATE1,
		PAYTYPID
from	
		Replication_PAM10501 pam
		inner join Agreement a on (a.agreementsubid = (LEFT(replace(pam.[jobnumbr], substring(pam.[jobnumbr], 1, charindex('_', pam.[jobnumbr])), ''), charindex('_', replace(pam.[jobnumbr], substring(pam.[jobnumbr], 1, charindex('_', pam.[jobnumbr])), '')) - 1))
		and a.agreementtype = (select pl.PicklistId from PickList pl inner join Picktype pt  on pt.picktypeid = pl.picktypeid  and pt.type = 'agreementtype'  and pl.Title = 'contract' where  pl.inactive = 0 and pt.inactive = 0 and pl.verticalid  = -1))

GO