select count(pk.EntityType) numberOf,  u.name, * from PlayerKills pk
join User u on pk.Xuid = u.Xuid
where KillTime like '%2025-09-10%' or KillTime like '%2025-09-09%'
group by pk.EntityType
order by u.name
