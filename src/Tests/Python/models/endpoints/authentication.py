#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json
import base64
from . import request_config

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
		self.response = requests.post(self.url, data = json.dumps(self.payload, ensure_ascii=False).encode('utf8'), headers = self.header, verify = request_config.VERIFY_HTTPS_REQUEST)

	def validate_token(self):
		token_payload = (self.token.split('.'))[1]
		# add padding for base64 decoding
		padding_needed = len(token_payload) % 4
		if padding_needed:
			token_payload += '=' * padding_needed

		decoded_payload = json.loads(base64.b64decode(token_payload).decode('utf-8'))

		validations = []
		validations.append(decoded_payload["unique_name"] == self.user.displayName)
		validations.append(decoded_payload["email"] == self.user.email)
		validations.append(decoded_payload["aud"] == self.user.level)

		return all(validation == True for validation in validations)

	def get_outcome(self):
		error_messages = []

		if self.response.status_code == 200:
			is_success = True
			self.token = self.response.json()["accessToken"]
			if not self.validate_token():
				error_messages.append({"Message": "Token is not valid"})
				is_success = False
		else:
			is_success = False
			try:
				response_body = self.response.json()
			except:
				response_body = {}

			if self.response.status_code == 401:
				error_messages.append({"Message": "Unauthorized"})
			if self.response.status_code == 400:
				error_messages.append({"Message": "Bad Request"})
			elif self.response.status_code == 500:
				error_messages.append({"Message": "Internal Server Error"})

		self.response_summary = ResponseSummary(is_success, error_messages, self.response.status_code)

		return self.response_summary

	def get_token(self):
		return self.token