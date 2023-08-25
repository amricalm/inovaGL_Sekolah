	select mak.kode_perkiraan, turunan, sum(dtl.mutasi_debet + isnull(tr1.debet,0)) debet , sum(dtl.mutasi_kredit+isnull(tr1.kredit,0)) kredit
	from ac_ms_perk mak
	left outer join ac_saldo dtl
		on mak.kode_perkiraan = dtl.kode_perkiraan
	left outer join
	(
		select mak.kode_perkiraan, mak.kode_induk, sum(dtl.mutasi_debet + isnull(tr2.debet,0)) debet , sum(dtl.mutasi_kredit+isnull(tr2.kredit,0)) kredit
		from ac_ms_perk mak
		left outer join ac_saldo dtl
			on mak.kode_perkiraan = dtl.kode_perkiraan
		left outer join
		(
			select mak.kode_perkiraan, mak.kode_induk,  sum(dtl.mutasi_debet + isnull(tr3.debet,0)) debet , sum(dtl.mutasi_kredit+isnull(tr3.kredit,0)) kredit
			from ac_ms_perk mak
			left outer join ac_saldo dtl
				on mak.kode_perkiraan = dtl.kode_perkiraan
			left outer join
			(
				select dtl.kode_perkiraan, mak.kode_induk, sum(mutasi_debet) debet, sum(mutasi_kredit) kredit
				from ac_saldo dtl
				inner join ac_ms_perk mak
				on dtl.kode_perkiraan = mak.kode_perkiraan
				where turunan = 3
				group by dtl.kode_perkiraan,kode_induk
			) tr3
			on mak.kode_perkiraan = tr3.kode_induk
			where turunan =2
			group by mak.kode_perkiraan, mak.kode_induk
		)tr2
		on mak.kode_perkiraan = tr2.kode_induk
		where turunan =1
		group by mak.kode_perkiraan, mak.kode_induk
	)tr1
	on mak.kode_perkiraan = tr1.kode_induk
	where turunan =0
	group by mak.kode_perkiraan,turunan
	
union

	select mak.kode_perkiraan, turunan, sum(dtl.mutasi_debet + isnull(tr2.debet,0)) debet , sum(dtl.mutasi_kredit+isnull(tr2.kredit,0)) kredit
		from ac_ms_perk mak
		left outer join ac_saldo dtl
			on mak.kode_perkiraan = dtl.kode_perkiraan
		left outer join
		(
			select mak.kode_perkiraan, mak.kode_induk,  sum(dtl.mutasi_debet + isnull(tr3.debet,0)) debet , sum(dtl.mutasi_kredit+isnull(tr3.kredit,0)) kredit
			from ac_ms_perk mak
			left outer join ac_saldo dtl
				on mak.kode_perkiraan = dtl.kode_perkiraan
			left outer join
			(
				select dtl.kode_perkiraan, mak.kode_induk, sum(mutasi_debet) debet, sum(mutasi_kredit) kredit
				from ac_saldo dtl
				inner join ac_ms_perk mak
				on dtl.kode_perkiraan = mak.kode_perkiraan
				where turunan = 3
				group by dtl.kode_perkiraan,kode_induk
			) tr3
			on mak.kode_perkiraan = tr3.kode_induk
			where turunan =2
			group by mak.kode_perkiraan, mak.kode_induk
		)tr2
		on mak.kode_perkiraan = tr2.kode_induk
		where turunan =1
		group by mak.kode_perkiraan,turunan

union

select mak.kode_perkiraan, mak.turunan,  sum(dtl.mutasi_debet + isnull(tr3.debet,0)) debet , sum(dtl.mutasi_kredit+isnull(tr3.kredit,0)) kredit
			from ac_ms_perk mak
			left outer join ac_saldo dtl
				on mak.kode_perkiraan = dtl.kode_perkiraan
			left outer join
			(
				select dtl.kode_perkiraan, mak.kode_induk, sum(mutasi_debet) debet, sum(mutasi_kredit) kredit
				from ac_saldo dtl
				inner join ac_ms_perk mak
				on dtl.kode_perkiraan = mak.kode_perkiraan
				where turunan = 3
				group by dtl.kode_perkiraan,kode_induk
			) tr3
			on mak.kode_perkiraan = tr3.kode_induk
			where turunan =2
			group by mak.kode_perkiraan, mak.turunan