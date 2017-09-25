#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json

from models.response_summary import ResponseSummary

class Authentication():
	def __init__(self, url, user):
		self.url = url
		self.user = user
		self.header = {"Content-Type": "application/json"}
		self.init_payload()
		self.fetch()

	def init_payload(self):
		self.payload = {"email": self.user.email, "password": self.user.password}

	def fetch(self):
		self.response = requests.post(self.url, data = json.dumps(self.payload, ensure_ascii=False).encode('utf8'), headers = self.header, verify = False)

	def get_outcome(self):
		error_messages = []

		if self.response.status_code == 200:
			is_success = True
			self.token = self.response.json()["accessToken"]
		else:
			is_success = False
			response_body = self.response.json()

			if self.response.status_code == 401:
				error_messages.append({"Message": "Unauthorized"})
			elif self.response.status_code == 500:
				error_messages.append({"Message": "Internal Server Error"})

		self.response_summary = ResponseSummary(is_success, error_messages, self.response.status_code)

		return self.response_summary

	def get_token(self):
		return self.token