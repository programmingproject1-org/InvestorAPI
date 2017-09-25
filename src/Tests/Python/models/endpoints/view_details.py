#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json
from . import request_config
from models.response_summary import ResponseSummary

class ViewDetails():
	def __init__(self, url, user, token):
		self.url = url
		self.user = user
		if token is None:
			token = ""
		self.token = token
		self.header = {"Content-Type": "application/json", "Authorization": "Bearer " + token}
		self.fetch()

	def fetch(self):
		self.response = requests.get(self.url, headers = self.header, verify = request_config.VERIFY_HTTPS_REQUEST)

	def get_outcome(self):
		error_messages = []

		if self.response.status_code == 200:
			is_success = True
			try:
				response_body = self.response.json()
				if len(response_body["accounts"]) != 1:
					error_messages.append({"Message": "User does not have exactly 1 account"})
				if response_body["displayName"] != self.user.displayName:
					error_messages.append({"Message": "displayName does not match"})
				if response_body["email"] != self.user.email:
					error_messages.append({"Message": "email does not match"})
				if response_body["level"] != self.user.level:
					error_messages.append({"Message": "level does not match"})
				if len(error_messages) > 0:
					is_success = False
			except:
				response_body = {}
		else:
			is_success = False
			
			try:
				response_body = self.response.json()
			except:
				response_body = {}

			if self.response.status_code == 401:
				error_messages.append({"Message": "Unauthorized"})

		self.response_summary = ResponseSummary(is_success, error_messages, self.response.status_code)

		return self.response_summary