drop table if exists users;
create table if not exists users (
	id bigserial primary key not null,
	name varchar not null,
	lastloggin timestamp,
	setings json
);
	