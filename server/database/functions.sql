drop function if exists auntification;
drop function if exists create_direct_conversation;

create function auntification (_login varchar,p_password varchar)
returns TEXT
LANGUAGE plpgsql 
SECURITY DEFINER
SET search_path = public
AS $$
DECLARE
    v_token TEXT;
	v_user_id bigint;
BEGIN
    v_user_id := (select id from users where login = _login and password_hash = crypt(p_password, password_hash));
	if v_user_id IS NULL THEN  RETURN NULL;END IF;
	v_token := encode(gen_random_bytes(32), 'base64');
	INSERT INTO sessions(user_id,token) values (v_user_id,v_token);
	if (select count(user_id) from sessions where user_id = v_user_id group by user_id) > 3 then 
		delete from sessions where created_at = (select min(created_at) from sessions where user_id = v_user_id) and user_id = v_user_id;
		end if;
	update users set last_login = now() where users.id = v_user_id ;
	return v_token;
END;
$$;


create function create_direct_conversation(i_token text ,interlocutor_id bigint)
returns bigint
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public
AS $$
declare 
	p_user_id bigint;
	p_conversations_id bigint;
BEGIN
	p_user_id := (select s.user_id from sessions as s where i_token = s.token);
	if p_user_id is null then raise exception 'invalid token'; end if;
	if not exists (select 1 from users where id = interlocutor_id) then raise exception 'interlocutor not found';end if;
	p_conversations_id := (select c.id from conversations c 
		join conversation_participants c0 ON c0.conversation_id = c.id AND c0.user_id = p_user_id
    	join conversation_participants c1 ON c1.conversation_id = c.id AND c1.user_id = interlocutor_id
    	where c.type = 'D');
	if p_conversations_id is null then
   		insert INTO conversations (type) 
	    	values ('D') returning id into p_conversations_id;
		insert into conversation_participants(conversation_id,user_id) 
			values (p_conversations_id,p_user_id);
		insert into conversation_participants(conversation_id,user_id) 
			values (p_conversations_id,interlocutor_id);
	end if;
	return p_conversations_id;
END;
$$;
