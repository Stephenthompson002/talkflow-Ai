-- Create tables if they don't exist
CREATE TABLE IF NOT EXISTS Conversations (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Title TEXT NOT NULL DEFAULT '',
    CustomerId TEXT NOT NULL DEFAULT '',
    CreatedAt TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS Messages (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    ConversationId UUID NOT NULL,
    Sender TEXT NOT NULL DEFAULT '',
    Content TEXT NOT NULL DEFAULT '',
    SentAt TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (ConversationId) REFERENCES Conversations(Id) ON DELETE CASCADE
);

-- Create index for better performance
CREATE INDEX IF NOT EXISTS idx_messages_conversationid ON Messages(ConversationId);