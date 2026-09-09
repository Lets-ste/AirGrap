drop table if exists messages;
CREATE TABLE messages (
    id BIGSERIAL PRIMARY KEY,
    conversation_id BIGINT NOT NULL REFERENCES conversations(id) ON DELETE CASCADE,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    message TEXT,
	reply_to_id     BIGINT REFERENCES messages(id),
    created_at TIMESTAMP NOT NULL DEFAULT now(),
    edited_at TIMESTAMP)