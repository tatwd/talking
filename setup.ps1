# docker network create dev

docker run --name test_mongo `
    -e MONGO_INITDB_ROOT_USERNAME=root `
    -e MONGO_INITDB_ROOT_PASSWORD=test123 `
    -p 27017:27017 `
    --network dev `
    --rm -d mongo