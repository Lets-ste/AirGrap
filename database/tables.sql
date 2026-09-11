drop table if exists messages;
drop table if exists conversation_participants;
drop table if exists conversations;
drop table if exists users;

create table users (
	id bigserial unique,
	login varchar unique,
	password_hash varchar not null,
	nickname varchar not null ,
	lastloggin timestamp,
	account_type char(1) not null default 'U' check (account_type in ('U','B','D')),
    created_at timestamp not null default now(),
	setings json,
	primary key (id,login)
);
create table conversations  (
	id bigserial primary key,
	name varchar,
	type char(1) not null check (type in ('D','G')),
	created_at timestamp not null default now()
);
create table conversation_participants (
	conversation_id BIGINT NOT NULL REFERENCES conversations(id) ON DELETE CASCADE,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
	user_role char(1) not null default 'U' check (user_role in ('U','A','B')),
	last_read_id BIGINT NOT NULL DEFAULT 0,
    joined_at TIMESTAMP NOT NULL DEFAULT now(),
    PRIMARY KEY (conversation_id, user_id) 
);
CREATE TABLE messages (
    id BIGSERIAL PRIMARY KEY,
    conversation_id BIGINT NOT NULL REFERENCES conversations(id) ON DELETE CASCADE,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    message TEXT,
	reply_to_id     BIGINT REFERENCES messages(id),
    created_at TIMESTAMP NOT NULL DEFAULT now(),
    edited_at TIMESTAMP
);
	