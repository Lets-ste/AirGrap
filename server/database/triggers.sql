CREATE OR REPLACE FUNCTION set_updated_at()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    NEW.updated_at := now();
    RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_set_updated_at ON conversations;
CREATE TRIGGER trigger_set_updated_at
    BEFORE UPDATE ON conversations
    FOR EACH ROW
    EXECUTE FUNCTION set_updated_at();

CREATE OR REPLACE FUNCTION log_row_change()
RETURNS TRIGGER
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public
AS $$
DECLARE
    v_old jsonb;
    v_new jsonb;
    v_user bigint;
BEGIN
    IF TG_OP <> 'INSERT' THEN
        v_old := to_jsonb(OLD);
        v_user := (v_old->>'user_id')::bigint;
    END IF;
    IF TG_OP <> 'DELETE' THEN
        v_new := to_jsonb(NEW);
        v_user := coalesce(v_user, (v_new->>'user_id')::bigint);
    END IF;
    INSERT INTO log(table_name, event_type, old_value, new_value, user_id)
    VALUES (TG_TABLE_NAME,
            CASE TG_OP WHEN 'INSERT' THEN 'I' WHEN 'UPDATE' THEN 'U' ELSE 'D' END,
            v_old, v_new, v_user);
    IF TG_OP = 'DELETE' THEN
        RETURN OLD;
    END IF;
    RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trigger_users_audit ON users;
CREATE TRIGGER trigger_users_audit
    AFTER INSERT OR UPDATE OR DELETE ON users
    FOR EACH ROW EXECUTE FUNCTION log_row_change();

DROP TRIGGER IF EXISTS trigger_sessions_audit ON sessions;
CREATE TRIGGER trigger_sessions_audit
    AFTER INSERT OR UPDATE OR DELETE ON sessions
    FOR EACH ROW EXECUTE FUNCTION log_row_change();

DROP TRIGGER IF EXISTS trigger_messages_audit ON messages;
CREATE TRIGGER trigger_messages_audit
    AFTER INSERT OR UPDATE OR DELETE ON messages
    FOR EACH ROW EXECUTE FUNCTION log_row_change();
