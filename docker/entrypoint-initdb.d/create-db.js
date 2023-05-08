db = db.getSiblingDB('lodge');
db.createUser({
    user: 'lodge1312',
    pwd: 'lodge420',
    roles: [
        {
            role: 'readWrite',
            db: 'lodge'
        }
    ]
});