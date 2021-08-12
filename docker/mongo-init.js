db.auth('root', 'root');

db = db.getSiblingDB('password_manager');

db.createUser({
  user: 'pm',
  pwd: 'pm',
  roles: [
    {
      role: 'root',
      db: 'password_manager',
    },
  ],
});
