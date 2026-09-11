drop FUNCTION auntification;
CREATE FUNCTION auntification (_login varchar,p_password varchar)
returns bigint
LANGUAGE plpgsql 
AS $$
BEGIN
    RETURN (select id from users where login = _login and password_hash = crypt(p_password, password_hash));
END;
$$;