create table conversations  (
	id bigserial primary key,
	name varchar,
	type char(1) not null check (type in ('D','G')),
	created_at timestamp not null default now()
)