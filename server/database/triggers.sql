CREATE TRIGGER trigger_set_updated_at
    BEFORE UPDATE ON products -- Срабатывает до выполнения UPDATE
    FOR EACH ROW               -- Для каждой изменяемой строки
    EXECUTE FUNCTION set_updated_at();