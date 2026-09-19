drop FUNCTION auntification;
CREATE FUNCTION auntification (_login varchar,p_password varchar)
returns TEXT
LANGUAGE plpgsql 
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
		delete from sessions where created_at = (select min(created_at) from sessions where user_id = v_user_id group by created_at limit 1);
		end if;
	update users set last_login = now() where users.id = v_user_id ;
	return v_token;
END;
$$;