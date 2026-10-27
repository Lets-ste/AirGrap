drop PROCEDURE if exists registration;
drop PROCEDURE if exists send_message(text,bigint,text);
drop PROCEDURE if exists send_message(text,bigint,text,bigint);

CREATE PROCEDURE registration(_login varchar, p_password varchar,_nickname varchar)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO users (login, password_hash,nickname) VALUES (_login, crypt(p_password,gen_salt('bf',10)),_nickname);
END;
$$;

create PROCEDURE send_message (i_token text,i_conversation_id bigint,i_message text)
LANGUAGE plpgsql
SECURITY DEFINER 
SET search_path = public
AS $$
declare
p_user_id bigint;
BEGIN
	 p_user_id := (select user_id from sessions where token = i_token);
	 if p_user_id is null 
	 	then raise exception 'session is not found'; end if ;
	 if not exists (select * from conversation_participants as c where c.conversation_id = i_conversation_id and c.user_id = p_user_id) then 
	 	raise exception 'conversation is not found'; end if ;
	 insert into messages(conversation_id,user_id,message) values (i_conversation_id,p_user_id,i_message);
END;
$$;

create PROCEDURE send_message (i_token text,i_conversation_id bigint,i_message text,i_reply_to_id bigint)
LANGUAGE plpgsql
SECURITY DEFINER 
SET search_path = public
AS $$
declare
p_user_id bigint;
BEGIN
	 p_user_id := (select user_id from sessions where token = i_token);
	 if p_user_id is null 
	 	then raise exception 'session is not found'; end if ;
	 if not exists (select * from conversation_participants as c where c.conversation_id = i_conversation_id and c.user_id = p_user_id) then 
	 	raise exception 'conversation is not found'; end if ;
	 if not exists (select * from messages as m where m.id = i_reply_to_id) then 
	 	raise exception 'conversation is not found'; end if ;
	 insert into messages(conversation_id,user_id,message,reply_to_id) values (i_conversation_id,p_user_id,i_message,i_reply_to_id);
END;
$$;