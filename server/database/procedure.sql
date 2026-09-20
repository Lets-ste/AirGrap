drop PROCEDURE if exists registration;

CREATE PROCEDURE registration(_login varchar, p_password varchar,_nickname varchar)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO users (login, password_hash,nickname) VALUES (_login, crypt(p_password,gen_salt('bf',10)),_nickname);
END;
$$;


/*create PROCEDURE send_message (i_token text,i_conversation_id bigint,i_message text)
LANGUAGE plpgsql
AS $$
BEGIN
	if (select * from conversation_participants as c INNER JOIN sessions as s on c.user_id = s.user_id and s.token = i_token)
END;*/