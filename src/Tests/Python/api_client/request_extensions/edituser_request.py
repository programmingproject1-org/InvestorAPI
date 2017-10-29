#!/usr/bin/env python
# -*- coding: utf-8 -*-

from requests import Request
import json

from api_client.response_wrappers.edituser_response_wrapper import EditUserResponseWrapper

class EditUserRequest(Request):

	URL = "https://investor-api.herokuapp.com/api/1.0/users"

	def __init__(self, session, token, displayName, email):
		self.token = token
		self.make_request(displayName, email)
		self.session = session
		self.displayName = displayName
		self.email = email

	def make_request(self, displayName, email):
		user_info = {"displayName": displayName, "email": email}
		headers = {
			"Content-Type": "application/json",
			"charset": "UTF-8",
			"Authorization": "Bearer " + str(self.token)
		}
		payload = json.dumps(user_info, ensure_ascii = False).encode('utf8')
		super().__init__('PUT', url = self.URL, headers = headers, data = payload)

	def get_response(self):
		response = self.session.send(super().prepare())
		wrapped_response = EditUserResponseWrapper(response)
		response.close()
		return wrapped_response