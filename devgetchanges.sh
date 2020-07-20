#!/bin/bash

cd ~

rm -fR Documents/workspace/microting/angular-my-microting-plugin/eform-client/src/app/plugins/modules/my-microting-pn

cp -av Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/my-microting-pn Documents/workspace/microting/angular-my-microting-plugin/eform-client/src/app/plugins/modules/my-microting-pn

rm -fR Documents/workspace/microting/angular-my-microting-plugin/eFormAPI/Plugins/MyMicroting.Pn

cp -av Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/MyMicroting.Pn Documents/workspace/microting/angular-my-microting-plugin/eFormAPI/Plugins/MyMicroting.Pn

# Test files rm

rm -fR Documents/workspace/microting/angular-my-microting-plugin/eform-client/e2e/Tests/my-microting-settings
rm -fR Documents/workspace/microting/angular-my-microting-plugin/eform-client/e2e/Tests/my-microting-general
rm -fR Documents/workspace/microting/angular-my-microting-plugin/eform-client/e2e/Assets/
rm -fR Documents/workspace/microting/angular-my-microting-plugin/eform-client/wdio-headless-plugin-step2.conf.js

# Test files cp

cp -av Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/my-microting-settings Documents/workspace/microting/angular-my-microting-plugin/eform-client/e2e/Tests/my-microting-settings
cp -av Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/my-microting-general Documents/workspace/microting/angular-my-microting-plugin/eform-client/e2e/Tests/my-microting-general
cp -av Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Assets Documents/workspace/microting/angular-my-microting-plugin/eform-client/e2e/Assets/
cp -av Documents/workspace/microting/eform-angular-frontend/eform-client/wdio-plugin-step2.conf.js Documents/workspace/microting/angular-my-microting-plugin/eform-client/wdio-headless-plugin-step2.conf.js
