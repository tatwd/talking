#!/bin/bash
rm -rf api/output
dotnet publish "api/Talking.Api.csproj" -c Release -o output \
    && cd api \
    && tar -zcvf release.tar.gz output/