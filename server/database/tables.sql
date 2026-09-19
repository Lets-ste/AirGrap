drop table if exists log;
drop table if exists messages;
drop table if exists conversation_participants;
drop table if exists conversations;
drop table if exists sessions;
drop table if exists users;
CREATE EXTENSION IF NOT EXISTS pgcrypto;
create table users (
	id bigserial primary key,
    login varchar unique not null,
	password_hash text not null,
	nickname varchar not null ,
	last_login timestamptz ,
	account_type char(1) not null default 'U' check (account_type in ('U','A')),
    created_at timestamptz  not null default now(),
	settings jsonb not null default '{}'::jsonb
);
create table conversations  (
	id bigserial primary key,
	name varchar not null,
	type char(1) not null check (type in ('D','G')),
	created_at timestamptz  not null default now(),
	updated_at timestamptz not null default now()
);
create table conversation_participants (
	conversation_id BIGINT not null references conversations(id) on delete cascade,
    user_id BIGINT not null references users(id) on delete cascade,
	user_role char(1) not null default 'U' check (user_role in ('U','A','B')),
	last_read_id BIGINT not null default 0,
    joined_at timestamptz  not null default now(),
    PRIMARY KEY (conversation_id, user_id) 
);
create table messages (
    id BIGSERIAL PRIMARY KEY,
    conversation_id BIGINT not null references conversations(id) on delete cascade,
    user_id BIGINT references users(id) on delete set null,
    message text,
	reply_to_id BIGINT references messages(id) on delete set null,
    created_at timestamptz not null default now(),
    edited_at timestamptz 
);
create table sessions (
    user_id bigint not null references users(id) on delete cascade,
	token text primary key, 
	created_at timestamptz not null default now(),
    last_activity_at timestamptz  not null default now()
);
create table log (
	table_name varchar not null,
	event_type char(1) check (event_type in ('D','U','I')),
	old_value jsonb,
    new_value jsonb,
	user_id bigint,
	created_at timestamptz not null default now()
);
create index idx_messages_conv_id on messages(conversation_id, id desc);
create index idx_participants_user on conversation_participants(user_id);
create index idx_sessions_user on sessions(user_id);
create index idx_sessions_last_activity on sessions(last_activity_at);
create index idx_conversations_updated on conversations(updated_at desc);