# TalkFlow AI – Voice + Text Customer Support Platform (Starter)

TalkFlow is a SaaS starter scaffold providing real-time chat (SignalR), speech-to-text placeholders (Whisper), GPT intent handling placeholders, SQL Server storage, Redis, Docker deployment, and an Angular dashboard.

## What's included
- backend/TalkFlow.Api: .NET 8 SignalR + API scaffold with placeholder services for Whisper and GPT intent handling.
- frontend: Angular 16 minimal chat UI connecting to SignalR.
- docker-compose.yml: SQL Server + Redis + API + frontend.
- CI workflow to build backend & frontend.


## Run locally (Docker)

```
docker-compose up --build
```

## Next steps
- Integrate with Whisper/OpenAI for speech-to-text (replace WhisperService).
- Implement real auth (JWT/Identity) and role-based access for agents/admins.
- Add persistence, migrations, monitoring, and tests.
