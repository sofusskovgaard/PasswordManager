# PasswordManager

This project is a simple password manager that allows users to create an account, login with the account and store encrypted passwords in a MongoDB database.

### In a perfect world...

In a perfect world this would have been a desktop application to keep the clear/non-encrypted passwords away from the server. By only encrypting and decrypting on the client using a keypair only stored on the client.

This way the passwords will never travel the scary no man's land that is the interwebs. This would also mean that the passwords could only be decrypted using the keypair on this singular client, thus defeating the purpose of storing them in a database on the internet. You wouldn't be able to decrypt them on any other clients without the correct keypair from the first client and sending the keypair anywhere without proper security precautions could put all passwords at risk. Thus making this a rather cumbersome project to create.

## Goals

- [X] Encryption of traffic from and to the server. (SSL/TSL)
- [X] Encryption of password entities stored in the database.
- [X] Hashing of passwords for users stored in the database.
- [X] Signing of Json Web Tokens to identify the client/user sending a request.
- [X] Using MongoDB as a data store.
- [ ] Only encrypting and decrypting passwords on the client, thus ensuring no clear passwords will ever leave the client. 

## Getting started

1. Install docker
2. Start a `mongo` container to act as data store.
   install docker and start a mongodb container.
    ```docker
    docker run -d --name mongo -e MONGO_INITDB_ROOT_USERNAME="root" -e MONGO_INITDB_ROOT_PASSWORD="root" -p 27017:27017 -p 27018:27018 mongo
    ```
