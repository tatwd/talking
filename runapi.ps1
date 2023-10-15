# must same network with mongodb

docker run --rm --name talkingapi-demo `
    -e MONGO_URL=mongodb://root:test123@test_mongo:27017 `
    -e MONGO_DB=talking_sys `
    --network dev `
    -p 8000:80 talkingapi 