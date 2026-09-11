drop PROCEDURE if exists registration;

CREATE PROCEDURE registration(_login varchar, p_password varchar,_nickname varchar)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO users (login, password_hash,nickname) VALUES (_login, crypt(p_password,gen_salt('bf',10)),_nickname);
END;
$$;