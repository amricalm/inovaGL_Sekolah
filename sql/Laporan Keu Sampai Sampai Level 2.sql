
	select mak.kd_akun,  sum(dtl.debet + isnull(tr1.debet,0)) debet , sum(dtl.kredit+isnull(tr1.kredit,0)) kredit
	from ac_makun mak
	left outer join ac_tkm_dtl dtl
		on mak.kd_akun = dtl.kd_akun
	left outer join
	(
		select mak.kd_akun, mak.kd_induk,  sum(dtl.debet + isnull(tr2.debet,0)) debet , sum(dtl.kredit+isnull(tr2.kredit,0)) kredit
		from ac_makun mak
		left outer join ac_tkm_dtl dtl
			on mak.kd_akun = dtl.kd_akun
		left outer join
		(
			select dtl.kd_akun, mak.kd_induk, sum(debet) debet, sum(kredit) kredit
			from ac_tkm_dtl dtl
			inner join ac_makun mak
			on dtl.kd_akun = mak.kd_akun
			where turunan = 2
			group by dtl.kd_akun,kd_induk
		) tr2
		on mak.kd_akun = tr2.kd_induk
		where turunan =1
		group by mak.kd_akun, mak.kd_induk
	)tr1
	on mak.kd_akun = tr1.kd_induk
	where turunan =0
	group by mak.kd_akun
	
union

select mak.kd_akun, sum(dtl.debet + isnull(tr2.debet,0)) debet , sum(dtl.kredit+isnull(tr2.kredit,0)) kredit
from ac_makun mak
left outer join ac_tkm_dtl dtl
	on mak.kd_akun = dtl.kd_akun
left outer join
(
	select dtl.kd_akun, mak.kd_induk, sum(debet) debet, sum(kredit) kredit
	from ac_tkm_dtl dtl
	inner join ac_makun mak
	on dtl.kd_akun = mak.kd_akun
	where turunan = 2
	group by dtl.kd_akun,kd_induk
) tr2
on mak.kd_akun = tr2.kd_induk
where turunan =1
group by mak.kd_akun