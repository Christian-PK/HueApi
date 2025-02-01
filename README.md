# HueApi
Basic application for calling Philips Hue endpoints. 

## Environment variables

### Required
- HUE_API_KEY
- HUE_BASE_URL (eg. https://192.168.1.100)

### Optional
- HUE_DEVICE_SEARCH_FILTER (Regex filter to find lights, if none provided then '(.*)' will be used)

## Build and run steps
- docker build -t hueapi -f .\Dockerfile .
- docker run -e HUE_API_KEY='APIKEY' -e HUE_BASE_URL='https://192.168.1.100' -e HUE_DEVICE_SEARCH_FILTER='Livingroom(.*)' -p 8080:8080 hueapi


## Api key
Call the endpoint /hue/apikey. First time you will get a response that tells you that link button is not pressed. 
Press the button on your Hue bridge and call again. This time you will get a response containing  username and client key. Use the client key as api key. 