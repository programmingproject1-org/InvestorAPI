#!/usr/bin/env python
# -*- coding: utf-8 -*-

import requests
import json
import base64
from . import request_config

class Authentication():
	def __init__(self, url, email, password):
		self.url = url
		self.header = {"Content-Type": "application/json"}
		self.init_payload(email, password)
		self.fetch()

	def init_payload(self, email, password):
		self.payload = {"email": email, "password": password}

	def fetch(self):
		self.response = requests.post(self.url, data = json.dumps(self.payload, ensure_ascii=False).encode('utf8'), headers = self.header, verify = request_config.VERIFY_HTTPS_REQUEST)

	# @staticmethod
	# def validate_token(self, token, displayName, email, level):
	# 	token_payload = (token.split('.'))[1]
	# 	# add padding for base64 decoding
	# 	padding_needed = len(token_payload) % 4
	# 	if padding_needed:
	# 		token_payload += '=' * padding_needed

	# 	decoded_payload = json.loads(base64.b64decode(token_payload).decode('utf-8'))

	# 	validations = []
	# 	validations.append(decoded_payload["unique_name"] == displayName)
	# 	validations.append(decoded_payload["email"] == email)
	# 	validations.append(decoded_payload["aud"] == level)

	# 	return all(validation == True for validation in validations)

	def get_outcome(self):

		return (self.response, self.get_token())

	def get_token(self):

		token = None
		error = None

		if self.response.text:
			try:
				response_body = self.response.json()
				token = response_body["accessToken"]
			except ValueError:
				pass
			except KeyError:
				token = None

		return token